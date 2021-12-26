import { useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import PageList from "../Generic/PageList";

export function ClientList() {
  const { clientId } = useParams();
  const definition = useDefinition("ClientForList");
  const [loading, elements] = useApiFetch("Clients", { method: "GET" }, []);
  const texts = {
    title: "Clients",
    add: "Add client",
  };

  return (
    <PageList
      definition={definition}
      loading={loading}
      elements={elements}
      selectedId={clientId}
      texts={texts}
    />
  );
}
