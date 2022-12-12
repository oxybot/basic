import { useLoaderData, useParams } from "react-router-dom";
import { useDefinition } from "../api";
import { useSorting } from "../helpers/sorting";
import PageList from "../Generic/PageList";
import "./card.scss";

export function EventList() {
  const { eventId } = useParams();
  const definition = useDefinition("EventForList");
  const texts = {
    title: "Events",
    add: "Add event",
  };

  const elements = useLoaderData();
  const [sorting, updateSorting] = useSorting();

  return (
    <PageList
      definition={definition}
      elements={elements}
      selectedId={eventId}
      texts={texts}
      newRole="time"
      sorting={sorting}
      setSorting={updateSorting}
    />
  );
}
