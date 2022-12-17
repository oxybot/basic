import { useParams, useRevalidator } from "react-router-dom";
import { useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";

export function GlobalDayOffEdit({ full = false }) {
  const revalidator = useRevalidator();
  const { dayOffId } = useParams();
  const definition = useDefinition("GlobalDayOffForEdit");
  const texts = {
    title: (e) => e.displayName,
    subTitle: "Edit a Day-Off",
  };

  function handleUpdate() {
    revalidator.revalidate();
  }

  return (
    <PageEdit
      definition={definition}
      texts={texts}
      full={full}
      baseApiUrl="GlobalDaysOff"
      entityId={dayOffId}
      onUpdate={handleUpdate}
    />
  );
}
