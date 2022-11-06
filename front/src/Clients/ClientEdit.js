import { useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";
import { retrieveAll } from "./slice";

export function ClientEdit({ full = false }) {
  const dispatch = useDispatch();
  const { clientId } = useParams();
  const definition = useDefinition("ClientForEdit");
  const [, entity] = useApiFetch(["Clients", clientId], { method: "GET" }, {});
  const texts = {
    title: entity.displayName,
    subTitle: "Edit a Client",
  };

  function handleUpdate() {
    dispatch(retrieveAll());
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
