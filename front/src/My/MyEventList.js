import { useLoaderData, useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";

export function MyEventList() {
  const { eventId } = useParams();
  const definition = useDefinition("EventForList");
  const elements = useLoaderData();
  if (definition) {
    definition.fields = definition.fields.filter((f) => f.name !== "user");
  }

  const texts = {
    title: "My Events",
    add: "Add a new Request",
  };

  return (
    <PageList
      listClassName="d-xs-card d-sm-card"
      definition={definition}
      elements={elements}
      selectedId={eventId}
      texts={texts}
    />
  );
}
