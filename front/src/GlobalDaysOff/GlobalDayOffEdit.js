import { useParams, useRevalidator } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";

export function GlobalDayOffEdit({ full = false }) {
  const revalidator = useRevalidator();
  const { dayOffId } = useParams();
  const definition = useDefinition("GlobalDayOffForEdit");
  const [, entity] = useApiFetch(["GlobalDaysOff", dayOffId], { method: "GET" }, {});
  const texts = {
    title: entity.displayName,
    subTitle: "Edit a Day-Off",
  };

  function handleUpdate() {
    revalidator.revalidate();
  }

  return (
    <PageEdit
      definition={definition}
      entity={entity}
      texts={texts}
      full={full}
      baseApiUrl="GlobalDaysOff"
      entityId={dayOffId}
      onUpdate={handleUpdate}
    />
  );
}
