import { useParams, useRevalidator } from "react-router-dom";
import { useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";
import ItemsForm from "./ItemsForm";

const transformDef = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "items");
  return d;
};

const transform = (e) => {
  const updated = { ...e, clientIdentifier: e.client.identifier };
  updated.items = e.items.map((i) => {
    const updatedItem = { ...i, productIdentifier: i.product?.identifier };
    delete updatedItem.product;
    return updatedItem;
  });

  delete updated.client;
  return updated;
};

export function AgreementEdit({ full = false }) {
  const revalidator = useRevalidator();
  const { agreementId } = useParams();
  const definition = useDefinition("AgreementForEdit", transformDef);

  const texts = {
    title: (e) => e.title,
    subTitle: "Edit an Agreement",
    "form-action": "Update",
  };

  function handleUpdate() {
    revalidator.revalidate();
  }

  return (
    <PageEdit
      definition={definition}
      texts={texts}
      full={full}
      baseApiUrl="Agreements"
      entityId={agreementId}
      extendedForm={(e, s, err) => <ItemsForm entity={e} setEntity={s} errors={err} />}
      onUpdate={handleUpdate}
      transform={transform}
    />
  );
}
