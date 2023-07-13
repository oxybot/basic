import { IconMinus, IconPlus } from "@tabler/icons";
import clsx from "clsx";
import { useDefinition } from "../api";
import EntityFieldInput from "../Generic/EntityFieldInput";

const transformDef = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "identifier");
  return d;
};

export default function DetailsForm({ entity, setEntity, errors = {} }) {
  const itemDefinition = useDefinition("BalanceItemForEdit", transformDef);
  const itemFields = itemDefinition?.fields;

  function handleAddItem() {
    let updated = { ...entity, details: [...(entity.details || [])] };
    updated.details.push({});
    setEntity(updated);
  }

  function handleRemoveItem(index) {
    let updated = { ...entity, details: [...(entity.details || [])] };
    updated.details.splice(index, 1);
    setEntity(updated);
  }

  return (
    <div className="card pb-3 col-lg-12">
      <div className="card-header">
        <h3 className="card-title me-auto">Items</h3>
        <button type="button" className="btn btn-icon btn-primary" onClick={handleAddItem}>
          <IconPlus />
        </button>
      </div>
      <table className="table card-table table-vcenter text-nowrap datatable table-hover">
        <thead>
          <tr>
            {itemFields &&
              itemFields.map((field, index) => (
                <th key={index} className={clsx({ required: field.required })}>
                  {field.displayName}
                </th>
              ))}
            <th className="w-1"></th>
          </tr>
        </thead>
        <tbody>
          {entity.details &&
            entity.details.map((item, index) => {
              function handleChangeItem(event) {
                const name = event.target.name;
                const value = event.target.value;
                const updated = { ...entity, details: [...entity.details] };
                updated.details[index][name] = value;
                setEntity(updated);
              }

              return (
                <tr key={index}>
                  {itemFields &&
                    itemFields.map((field, fieldIndex) => (
                      <td key={fieldIndex}>
                        <EntityFieldInput
                          field={field}
                          value={item[field.name] || ""}
                          hasErrors={errors[`details[${index}].${field.name}`] !== undefined}
                          onChange={handleChangeItem}
                        />
                      </td>
                    ))}
                  <td>
                    <button
                      type="button"
                      className="btn btn-outline-primary btn-sm btn-icon"
                      onClick={() => handleRemoveItem(index)}
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
