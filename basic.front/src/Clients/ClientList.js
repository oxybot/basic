import { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import { clientsState, disconnect, getAll } from "./slice";
import PageList from "../Generic/PageList";

export function ClientList() {
  const dispatch = useDispatch();
  const { clientId } = useParams();
  const definition = useDefinition("ClientForList");
  const texts = {
    title: "Clients",
    add: "Add client",
  };
  const { loading, values: elements } = useSelector(clientsState);

  useEffect(() => {
    dispatch(getAll());
    return () => dispatch(disconnect());
  }, [dispatch]);

  return <PageList definition={definition} loading={loading} elements={elements} selectedId={clientId} texts={texts} />;
}
