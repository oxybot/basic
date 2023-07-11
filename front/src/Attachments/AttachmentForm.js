import { IconMinus, IconPlus } from "@tabler/icons-react";
import clsx from "clsx";
import { useDefinition } from "../api";
import EntityFieldInput from "../Generic/EntityFieldInput";

export default function AttachmentForm({ entity, setEntity, errors = {} }) {
  const attachmentDefinition = useDefinition("AttachmentForEdit");
  const attachmentFields = attachmentDefinition?.fields;

  function handleAddAttachment() {
    let updated = { ...entity, attachments: [...(entity.attachments || [])] };
    updated.attachments.push({});
    setEntity(updated);
  }

  function handleRemoveAttachment() {
    let updated = { ...entity, attachments: [...(entity.attachments || [])] };
    updated.attachments.pop({});
    setEntity(updated);
  }

  return (
    <div className="card pb-3 col-lg-12">
      <div className="card-header">
        <h3 className="card-title me-auto">Attachments</h3>
        <button type="button" className="btn btn-icon btn-primary" onClick={handleAddAttachment}>
          <IconPlus />
        </button>
      </div>
      <table className="table card-table table-vcenter text-nowrap datatable table-hover">
        <thead>
          <tr>
            {attachmentFields &&
              attachmentFields.map((field, index) => (
                <th key={index} className={clsx({ required: field.required })}>
                  {field.displayName}
                </th>
              ))}
            <th className="w-1"></th>
          </tr>
        </thead>
        <tbody>
          {entity.attachments &&
            entity.attachments.map((attachment, index) => {
              function handleChangeAttachment(event) {
                const name = event.target.name;
                const value = event.target.value;
                const updated = { ...entity, attachments: [...entity.attachments] };
                updated.attachments[index][name] = value;
                setEntity(updated);
              }

              return (
                <tr key={index}>
                  {attachmentFields &&
                    attachmentFields.map((field, fieldIndex) => (
                      <td key={fieldIndex}>
                        <EntityFieldInput
                          field={field}
                          value={attachment[field.name] || ""}
                          hasErrors={errors[`attachments[${index}].${field.name}`] !== undefined}
                          onChange={handleChangeAttachment}
                        />
                      </td>
                    ))}
                  <td>
                    <button
                      type="button"
                      className="btn btn-outline-primary btn-sm btn-icon"
                      onClick={() => handleRemoveAttachment()}
                    >
                      <IconMinus />
                    </button>
                  </td>
                </tr>
              );
            })}
        </tbody>
      </table>
    </div>
  );
}
