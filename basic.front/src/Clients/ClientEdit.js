import { apiUrl, useApiFetch, useDefinition } from "../api";
import { useParams } from "react-router-dom";
import PageEdit from "../Generic/PageEdit";

export function ClientEdit({full = false}) {
  const { clientId } = useParams();
  const definition = useDefinition("ClientForEdit");
  const [, entity] = useApiFetch(
    apiUrl("Clients", clientId),
    { method: "GET" },
    {}
  );
  const texts = {
    title: entity.displayName,
    subTitle: "Edit a Client",
  };

  return (
    <PageEdit
      definition={definition}
      entity={entity}
      texts={texts}
      full={full}
      baseApiUrl="Clients"
      entityId={clientId}
    />
  );
}
