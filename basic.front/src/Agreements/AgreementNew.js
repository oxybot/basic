import { useDispatch } from "react-redux";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import { refresh } from "./slice";

export function AgreementNew() {
  const dispatch = useDispatch();
  const definition = useDefinition("AgreementForEdit");

  const texts = {
    title: "Agreements",
    subTitle: "Add a new Agreement",
  };

  function handleCreate() {
    dispatch(refresh());
  }

  return (
    <PageNew
      definition={definition}
      baseApiUrl="Agreements"
      texts={texts}
      onCreate={handleCreate}
    />
  );
}
