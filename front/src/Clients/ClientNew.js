import { useRevalidator } from "react-router-dom";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";

export function ClientNew() {
  const revalidator = useRevalidator();
  const definition = useDefinition("ClientForEdit");
  const texts = {
    title: "Clients",
    subTitle: "Add a new Client",
  };

  function handleCreate() {
    revalidator.revalidate();
  }

  return <PageNew definition={definition} baseApiUrl="Clients" texts={texts} onCreate={handleCreate} />;
}
