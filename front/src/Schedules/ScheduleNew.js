import { useRevalidator } from "react-router-dom";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";

export function ScheduleNew() {
  const revalidator = useRevalidator();
  const definition = useDefinition("ScheduleForEdit");
  const texts = {
    title: "Schedules",
    subTitle: "Add a new Schedule",
  };

  function handleCreate() {
    revalidator.revalidate();
  }

  return <PageNew definition={definition} baseApiUrl="Schedules" texts={texts} onCreate={handleCreate} />;
}
