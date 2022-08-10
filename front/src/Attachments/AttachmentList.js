import { IconLoader, IconDownload } from "@tabler/icons";
import clsx from "clsx";
import EntityFieldView from "../Generic/EntityFieldView";
import { apiFetch } from "../api";
import { saveAs } from 'file-saver';
import { useState } from "react";

function filtered(fields) {
  if (!fields) {
    return fields;
  }
  
  return fields.filter((i) => i.type !== "key");
}

export default function AttachmentList({ loading, definition, entities, baseTo = null, selectedId, parentId }) {
  
  const [attachment, setAttachment] = useState();
  const fields = filtered(definition?.fields);

  const b64toBlob = (b64Data, contentType='', sliceSize=512) => {
    const byteCharacters = atob(b64Data);
    const byteArrays = [];
  
    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      const slice = byteCharacters.slice(offset, offset + sliceSize);
  
      const byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }
  
      const byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }
  
    const blob = new Blob(byteArrays, {type: contentType});
    return blob;
  }

  function getAttachment(attachmentId, parentId) {
    const get = { method: "GET" };
    console.log(attachmentId + " + " + parentId)

    apiFetch(["users", parentId, "attachments", attachmentId], get, {})
      .then((attachmentFromDb) => {
        setAttachment(attachmentFromDb);
      })
      console.log(attachment);
      var dataForFile = b64toBlob(attachment.attachmentContent.data, attachment.attachmentContent.mimeType);
      var file = new File([dataForFile], attachment.displayName, { type: attachment.attachmentContent.mimeType})

      console.log(file);
      saveAs(file);
  }

  function deleteAttachment(attachmentId, parentId) {
    const get = { method: "DELETE" };

    apiFetch(["users", {parentId}, "attachments", {attachmentId}], get, {})
  }
  
  return (
    <div className="table-responsive">
      <table className="table card-table table-vcenter text-nowrap datatable table-hover">
        <tbody>
          <tr className={loading ? "" : "d-none"}>
            <td colSpan={fields?.length}>
              <IconLoader /> Loading...
            </td>
          </tr>
          {entities.map((entity) => ( 
            <tr
              key={entity.identifier}
              className={clsx({
                "table-active": entity.identifier === selectedId,
              })}
            >
              {entity && fields &&
                <>
                  <td>
                    <EntityFieldView type={fields[0].type} value={entity[fields[0].name]} list />
                    <button className="btn btn-tertiary" onClick={() => getAttachment(entity.identifier, parentId)}>
                      < IconDownload />
                    </button>
                  </td>
                </>
              }
            </tr>
          ))}
          {!loading && entities.length === 0 && (
            <tr>
              <td colSpan={fields?.length}>
                <em>No results</em>
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
