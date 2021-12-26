import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";

export function ClientNew() {
  const definition = useDefinition("ClientForEdit");
  const texts = {
    title: "Clients",
    subTitle: "Add a new Client",
  };

  return <PageNew definition={definition} baseApiUrl="Clients" texts={texts} />;
}
