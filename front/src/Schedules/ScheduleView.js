import { useLoaderData } from "react-router-dom";
import { useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import PageView from "../Generic/PageView";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";

function ScheduleViewDetail() {
  const definition = useDefinition("ScheduleForView");
  const entity = useLoaderData();

  return <EntityDetail definition={definition} entity={entity} />;
}

export function ScheduleView({ backTo = null, full = false }) {
  const entity = useLoaderData();

  return (
    <PageView backTo={backTo} full={full} entity={entity} title={entity?.user?.displayName || "-"} editRole="schedules">
      <Sections>
        <Section code="detail" element={<ScheduleViewDetail />}>
          Detail
        </Section>
      </Sections>
    </PageView>
  );
}
