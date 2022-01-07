import { useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import PageView from "../Generic/PageView";
import Sections from "../Generic/Sections";
import Section from "../Generic/Section";

function ScheduleViewDetail({ entity }) {
  const definition = useDefinition("ScheduleForView");
  return <EntityDetail definition={definition} entity={entity} />;
}

export function ScheduleView({ backTo = null, full = false }) {
  const { scheduleId } = useParams();
  const get = { method: "GET" };
  const [, entity] = useApiFetch(["Schedules", scheduleId], get, {});

  return (
    <PageView backTo={backTo} full={full} entity={entity} title={entity?.user?.displayName || "-"} editRole="time">
      <Sections>
        <Section code="detail" element={<ScheduleViewDetail entity={entity} />}>
          Detail
        </Section>
      </Sections>
    </PageView>
  );
}
