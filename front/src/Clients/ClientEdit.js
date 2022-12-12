import { useParams, useRevalidator } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";

export function ClientEdit({ full = false }) {
  const revalidator = useRevalidator();
  const { clientId } = useParams();
  const definition = useDefinition("ClientForEdit");
  const [, entity] = useApiFetch(["Clients", clientId], { method: "GET" }, {});
  const texts = {
    title: entity.displayName,
    subTitle: "Edit a Client",
  };

  function handleUpdate() {
    revalidator.revalidate();
  }

  return (
    <PageEdit
      definition={definition}
      entity={entity}
      texts={texts}
      full={full}
      baseApiUrl="Clients"
      entityId={clientId}
      onUpdate={handleUpdate}
    />
  );
}
