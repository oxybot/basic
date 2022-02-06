import { IconMinus, IconPlus } from "@tabler/icons";
import clsx from "clsx";
import { useDispatch } from "react-redux";
import { useDefinition } from "../api";
import EntityFieldInput from "../Generic/EntityFieldInput";
import PageNew from "../Generic/PageNew";
import { refresh } from "./slice";

function CardForItems({ entity, setEntity, errors = {} }) {
  const itemDefinition = useDefinition("AgreementItemForEdit");
  const itemFields = itemDefinition?.fields;

  function handleAddItem() {
    let updated = { ...entity, items: [...(entity.items || [])] };
    updated.items.push({});
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
          {entity.items &&
            entity.items.map((item, index) => {
              function handleChangeItem(event) {
                const name = event.target.name;
                const value = event.target.value;
                const updated = { ...entity, items: [...entity.items] };
                updated.items[index][name] = value;
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
                          hasErrors={errors[`items[${index}].${field.name}`] !== undefined}
                          onChange={handleChangeItem}
                        />
                      </td>
                    ))}
                  <td>
                    <button type="button" className="btn btn-outline-primary btn-sm btn-icon">
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

const transform = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "items");
  return d;
};

export function AgreementNew() {
  const dispatch = useDispatch();
  const definition = useDefinition("AgreementForEdit", transform);

  const texts = {
    title: "Agreements",
    subTitle: "Add a new Agreement",
    "form-action": "Create",
  };

  function handleCreate() {
    dispatch(refresh());
  }

  return (
    <PageNew
      definition={definition}
      baseApiUrl="Agreements"
      texts={texts}
      extendedForm={(e, s, err) => <CardForItems entity={e} setEntity={s} errors={err} />}
      onCreate={handleCreate}
    />
  );
}
