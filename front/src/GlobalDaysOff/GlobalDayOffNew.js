import { useRevalidator } from "react-router-dom";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";

export function GlobalDayOffNew() {
  const revalidator = useRevalidator();
  const definition = useDefinition("GlobalDayOffForEdit");
  const texts = {
    title: "Global Days-Off",
    subTitle: "Add a new Day-Off",
  };

  function handleCreate() {
    revalidator.revalidate();
  }

  return <PageNew definition={definition} baseApiUrl="GlobalDaysOff" texts={texts} onCreate={handleCreate} />;
}
