import { useRevalidator } from "react-router-dom";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import ItemsForm from "./ItemsForm";

const transform = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "items");
  return d;
};

export function AgreementNew() {
  const revalidator = useRevalidator();
  const definition = useDefinition("AgreementForEdit", transform);
  const texts = {
    title: "Agreements",
    subTitle: "Add a new Agreement",
    "form-action": "Create",
  };

  function handleCreate() {
    revalidator.revalidate();
  }

  return (
    <PageNew
      definition={definition}
      baseApiUrl="Agreements"
      texts={texts}
      extendedForm={(e, s, err) => <ItemsForm entity={e} setEntity={s} errors={err} />}
      onCreate={handleCreate}
    />
  );
}
