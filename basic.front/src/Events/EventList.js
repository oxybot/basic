import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";
import { disconnect, getAll, eventsState } from "./slice";

export function EventList() {
  const dispatch = useDispatch();
  const { eventId } = useParams();
  const definition = useDefinition("EventForList");
  const texts = {
    title: "Events",
    add: "Add event",
  };

  const { loading, values: elements } = useSelector(eventsState);

  useEffect(() => {
    dispatch(getAll());
    return () => dispatch(disconnect());
  }, [dispatch]);

  return <PageList definition={definition} loading={loading} elements={elements} selectedId={eventId} texts={texts} />;
}
