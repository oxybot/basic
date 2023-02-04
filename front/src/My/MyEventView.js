import dayjs from "dayjs";
import { useLoaderData, useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import EntityList from "../Generic/EntityList";
import PageView from "../Generic/PageView";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";

function EventViewDetail() {
  const definition = useDefinition("EventForView");
  const entity = useLoaderData();

  if (definition) {
    definition.fields = definition.fields.filter((f) => f.name !== "user");
  }

  return <EntityDetail definition={definition} entity={entity} />;
}

function StatusList({ eventId }) {
  const definition = useDefinition("ModelStatusForList");
  const [loading, elements] = useApiFetch(["My/Events", eventId, "Statuses"], { method: "GET" }, []);
  return (
    <div className="card">
      <EntityList loading={loading} definition={definition} elements={elements.values} />
    </div>
  );
}

export function MyEventView({ backTo = null, full = false }) {
  const { eventId } = useParams();
  const entity = useLoaderData();

  return (
    <PageView
      backTo={backTo}
      full={full}
      entity={entity}
      title={dayjs(entity.startDate).format("DD MMM YYYY")}
      editRole="noedit"
    >
      <Sections>
        <Section code="detail" element={<EventViewDetail />}>
          Detail
        </Section>
        <Section code="statuses" element={<StatusList eventId={eventId} />}>
          Statuses
        </Section>
      </Sections>
    </PageView>
  );
}
