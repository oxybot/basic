import { IconPlus, IconMinus } from "@tabler/icons";
import { useCallback, useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { useNavigate, useParams } from "react-router-dom";
import { apiFetch, useDefinition } from "../api";
import EntityFieldInput from "../Generic/EntityFieldInput";
import EntityForm from "../Generic/EntityForm";
import { refresh } from "./slice";

const transform = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "items");
  return d;
};

export function AgreementEdit({ full = false }) {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const { agreementId } = useParams();
  const definition = useDefinition("AgreementForEdit", transform);
  const itemDefinition = useDefinition("AgreementItemForEdit");
  const [entity, setEntity] = useState({ items: [] });
  const [validated, setValidated] = useState(false);
  const itemFields = itemDefinition?.fields;

  const texts = {
    title: (e) => e.title,
    subTitle: "Edit an Agreement",
    "form-action": "Update",
  };

  const transform = useCallback((e) => {
    const updated = { ...e, clientIdentifier: e.client.identifier };
    updated.items = e.items.map((i) => {
      const updatedItem = { ...i, productIdentifier: i.product?.identifier };
      delete updatedItem.product;
      return updatedItem;
    });

    delete updated.client;
    return updated;
  }, []);

  useEffect(() => {
    apiFetch(["Agreements", agreementId], { method: "GET" })
      .then((response) => {
        setEntity(transform(response));
      })
      .catch((err) => console.log(err));
  }, [agreementId, transform]);

  const handleChange = (event) => {
    const name = event.target.name;
    const value = event.target.value;
    setEntity({ ...entity, [name]: value });
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    setValidated(true);
    apiFetch(["Agreements", agreementId], {
      method: "PUT",
      body: JSON.stringify(entity),
    })
      .then((response) => {
        navigate("./..");
        dispatch(refresh());
      })
      .catch((err) => {
        console.error(err);
        alert(err);
      });
  };

  function handleAddItem() {
    const updated = { ...entity, items: [...entity.items] };
    updated.items.push({});
    setEntity(updated);
  }

  return (
    <EntityForm
      definition={definition}
      entity={entity}
      full={full}
      texts={texts}
      handleChange={handleChange}
      handleSubmit={handleSubmit}
      validated={validated}
    >
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
              {itemFields && itemFields.map((field, index) => <th key={index}>{field.displayName}</th>)}
              <th className="w-1"></th>
            </tr>
          </thead>
          <tbody>
            {entity.items.map((item, index) => {
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
                    itemFields.map((field, index) => (
                      <td key={index}>
                        <EntityFieldInput field={field} value={item[field.name] || ""} onChange={handleChangeItem} />
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
    </EntityForm>
  );
}
