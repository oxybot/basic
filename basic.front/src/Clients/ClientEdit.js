import { useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { apiUrl, useApiFetch, useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";
import { refresh } from "./slice";

export function ClientEdit({ full = false }) {
  const dispatch = useDispatch();
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

  function handleUpdate() {
    dispatch(refresh());
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
