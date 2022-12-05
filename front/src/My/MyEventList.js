import { useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import PageList from "../Generic/PageList";

export function MyEventList() {
  const { eventId } = useParams();
  const definition = useDefinition("EventForList");
  if (definition) {
    definition.fields = definition.fields.filter((f) => f.name !== "user");
  }

  const texts = {
    title: "Events",
    add: "Add a new Request",
  };

  const [loading, elements] = useApiFetch("My/Events", { method: "GET" }, []);

  return (
    <PageList
      listClassName="d-xs-card d-sm-card"
      definition={definition}
      loading={loading}
      elements={elements}
      selectedId={eventId}
      texts={texts}
    />
  );
}
