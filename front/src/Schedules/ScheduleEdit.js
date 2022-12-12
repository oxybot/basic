import { useParams, useRevalidator } from "react-router-dom";
import { useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";

function transform(e) {
  let updated = { ...e, userIdentifier: e.user.identifier };
  delete updated.user;
  return updated;
}

export function ScheduleEdit({ full = false }) {
  const revalidator = useRevalidator();
  const { scheduleId } = useParams();
  const definition = useDefinition("ScheduleForEdit");
  const texts = {
    title: (e) => e?.user?.displayName || "-",
    subTitle: "Edit a Working Schedule",
  };

  function handleUpdate() {
    revalidator.revalidate();
  }

  return (
    <PageEdit
      definition={definition}
      texts={texts}
      full={full}
      baseApiUrl="Schedules"
      entityId={scheduleId}
      onUpdate={handleUpdate}
      transform={transform}
    />
  );
}
