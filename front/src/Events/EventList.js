import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";
import { disconnect, eventsState, setSorting, retrieveAll } from "./slice";

export function EventList() {
  const dispatch = useDispatch();
  const { eventId } = useParams();
  const definition = useDefinition("EventForList");
  const texts = {
    title: "Events",
    add: "Add event",
  };

  const { loading, elements, sorting } = useSelector(eventsState);

  useEffect(() => {
    dispatch(retrieveAll());
    return () => dispatch(disconnect());
  }, [dispatch, sorting]);

  return (
    <PageList
      definition={definition}
      loading={loading}
      elements={elements}
      selectedId={eventId}
      texts={texts}
      newRole="time"
      sorting={sorting}
      setSorting={(s) => dispatch(setSorting(s))}
    />
  );
}
