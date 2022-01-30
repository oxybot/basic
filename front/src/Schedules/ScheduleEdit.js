import { useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";
import { refresh } from "./slice";

function transform(e) {
  let updated = { ...e, userIdentifier: e.user.identifier };
  delete updated.user;
  return updated;
}

export function ScheduleEdit({ full = false }) {
  const dispatch = useDispatch();
  const { scheduleId } = useParams();
  const definition = useDefinition("ScheduleForEdit");
  const texts = {
    title: (e) => e?.user?.displayName || "-",
    subTitle: "Edit a Working Schedule",
  };

  function handleUpdate() {
    dispatch(refresh());
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
