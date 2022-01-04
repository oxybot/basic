import { useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import PageView from "../Generic/PageView";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";

function EventViewDetail({ entity }) {
  const definition = useDefinition("EventForView");
  return <EntityDetail definition={definition} entity={entity} />;
}

export function EventView({ backTo = null, full = false }) {
  const { eventId } = useParams();
  const get = { method: "GET" };
  const [, entity] = useApiFetch(["Events", eventId], get, {});

  return (
    <PageView backTo={backTo} full={full} entity={entity} editRole="time">
      <Sections>
        <Section code="detail" element={<EventViewDetail entity={entity} />}>
          Detail
        </Section>
      </Sections>
    </PageView>
  );
}
