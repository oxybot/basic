import { useDispatch } from "react-redux";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import { refresh } from "./slice";

export function ClientNew() {
  const dispatch = useDispatch();
  const definition = useDefinition("ClientForEdit");
  const texts = {
    title: "Clients",
    subTitle: "Add a new Client",
  };

  function handleCreate() {
    dispatch(refresh());
  }
  return (
    <PageNew
      definition={definition}
      baseApiUrl="Clients"
      texts={texts}
      onCreate={handleCreate}
    />
  );
}
