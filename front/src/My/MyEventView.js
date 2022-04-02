import { useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import EntityList from "../Generic/EntityList";
import PageView from "../Generic/PageView";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";

function EventViewDetail({ entity }) {
  const definition = useDefinition("EventForView");
  return <EntityDetail definition={definition} entity={entity} />;
}

function StatusList({ eventId }) {
  const definition = useDefinition("ModelStatusForList");
  const [loading, elements] = useApiFetch(["My/Events", eventId, "Statuses"], { method: "GET" }, []);
  return (
    <div className="card">
      <EntityList loading={loading} definition={definition} entities={elements} baseTo="/agreement" />
    </div>
  );
}

export function MyEventView({ backTo = null, full = false }) {
  const { eventId } = useParams();
  const get = { method: "GET" };
  const [, entity] = useApiFetch(["My/Events", eventId], get, {});

  return (
    <PageView backTo={backTo} full={full} entity={entity} editRole="noedit">
      <Sections>
        <Section code="detail" element={<EventViewDetail entity={entity} />}>
          Detail
        </Section>
        <Section code="statuses" element={<StatusList eventId={eventId} />}>
          Statuses
        </Section>
      </Sections>
    </PageView>
  );
}
