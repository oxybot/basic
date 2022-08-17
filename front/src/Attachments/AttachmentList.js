import { IconLoader, IconDownload, IconCircleX } from "@tabler/icons";
import clsx from "clsx";
import EntityFieldView from "../Generic/EntityFieldView";
import { apiFetch } from "../api";
import { saveAs } from "file-saver";
import { useState } from "react";

function filtered(fields) {
  if (!fields) {
    return fields;
  }

  return fields.filter((i) => i.type !== "key");
}

export default function AttachmentList({
  loading,
  definition,
  entities,
  baseTo = null,
  selectedId,
  typeOfParent,
  parentId,
}) {
  const [attachment, setAttachment] = useState();
  const fields = filtered(definition?.fields);

  const b64toBlob = (b64Data, contentType = "", sliceSize = 512) => {
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

    const blob = new Blob(byteArrays, { type: contentType });
    return blob;
  };

  function getAttachment(attachmentId) {
    const get = { method: "GET" };
    console.log(attachmentId + " + " + parentId);

    apiFetch([typeOfParent, parentId, "attachments", attachmentId], get, {}).then((attachmentFromDb) => {
      setAttachment(attachmentFromDb);
    });
    console.log(attachment);
    var dataForFile = b64toBlob(attachment.attachmentContent.data, attachment.attachmentContent.mimeType);
    var file = new File([dataForFile], attachment.displayName, { type: attachment.attachmentContent.mimeType });

    console.log(file);
    saveAs(file);
  }

  function deleteAttachment(attachmentId) {
    const deleteMethod = { method: "DELETE" };
    apiFetch([typeOfParent, parentId, "attachments", attachmentId], deleteMethod, {});
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
              {entity && fields && (
                <>
                  <td>
                    <button className="btn btn-tertiary" onClick={() => getAttachment(entity.identifier)}>
                      <EntityFieldView type={fields[0].type} value={entity[fields[0].name]} list /> <IconDownload />
                    </button>
                    <button
                      className="btn btn-danger"
                      onClick={() => {
                        if (window.confirm("Are you sure you want to delete this attachment?")) {
                          deleteAttachment(entity.identifier);
                        }
                      }}
                    >
                      <IconCircleX />
                    </button>
                  </td>
                </>
              )}
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
