import { useParams, useRevalidator } from "react-router-dom";
import { useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";

export function ClientEdit({ full = false }) {
  const revalidator = useRevalidator();
  const { clientId } = useParams();
  const definition = useDefinition("ClientForEdit");
  const texts = {
    title: (e) => e.displayName,
    subTitle: "Edit a Client",
  };

  function handleUpdate() {
    revalidator.revalidate();
  }

  return (
    <PageEdit
      definition={definition}
      texts={texts}
      full={full}
      baseApiUrl="Clients"
      entityId={clientId}
      onUpdate={handleUpdate}
    />
  );
}
