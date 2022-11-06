import { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";
import { clientsState, disconnect, retrieveAll, setSorting } from "./slice";

export function ClientList() {
  const dispatch = useDispatch();
  const { clientId } = useParams();
  const definition = useDefinition("ClientForList");
  const texts = {
    title: "Clients",
    add: "Add client",
  };

  const { loading, elements, sorting } = useSelector(clientsState);

  useEffect(() => {
    dispatch(retrieveAll());
    return () => dispatch(disconnect());
  }, [dispatch, sorting]);

  return (
    <PageList
      definition={definition}
      loading={loading}
      elements={elements}
      selectedId={clientId}
      texts={texts}
      newRole="client"
      sorting={sorting}
      setSorting={(s) => dispatch(setSorting(s))}
    />
  );
}
