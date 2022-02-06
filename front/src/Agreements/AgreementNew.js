import { useDispatch } from "react-redux";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import ItemsForm from "./ItemsForm";
import { refresh } from "./slice";

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
      extendedForm={(e, s, err) => <ItemsForm entity={e} setEntity={s} errors={err} />}
      onCreate={handleCreate}
    />
  );
}
