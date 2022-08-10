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

export default function EntityList({ loading, definition, entities, baseTo = null, selectedId }) {

  const [attachment, setAttachment] = useState();

  function GetAttachment(attachmentId) {
    const get = { method: "GET" };
    apiFetch(["attachment", attachmentId], get, {})
      .then((attachmentFromDb) => {
        setAttachment(attachmentFromDb);
      })
      console.log(attachment);
      var file = new File([attachment.attachmentContent], attachment.displayName, {type: attachment.attachmentContent.mimeType});
      console.log(file);
      saveAs(file);
  }

  const fields = filtered(definition?.fields);
  
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
                    <button className="btn btn-primary" onClick={() => GetAttachment(entity.identifier)}>
                      < IconDownload />
                    <EntityFieldView type={fields[0].type} value={entity[fields[0].name]} list />
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
