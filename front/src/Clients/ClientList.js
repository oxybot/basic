import { useLoaderData, useParams } from "react-router-dom";
import { useDefinition } from "../api";
import { useSorting } from "../helpers/sorting";
import PageList from "../Generic/PageList";

export function ClientList() {
  const { clientId } = useParams();
  const definition = useDefinition("ClientForList");
  const texts = {
    title: "Clients",
    add: "Add client",
  };

  const elements = useLoaderData();
  const [sorting, updateSorting] = useSorting();

  return (
    <PageList
      definition={definition}
      elements={elements}
      selectedId={clientId}
      texts={texts}
      newRole="clients"
      sorting={sorting}
      setSorting={updateSorting}
    />
  );
}
