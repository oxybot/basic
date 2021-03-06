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
  };

  const [loading, elements] = useApiFetch("My/Events", { method: "GET" }, []);

  return (
    <PageList
      definition={definition}
      loading={loading}
      elements={elements}
      selectedId={eventId}
      texts={texts}
      newRole=""
    />
  );
}
