import { useDispatch } from "react-redux";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import { retrieveAll } from "./slice";

export function ScheduleNew() {
  const dispatch = useDispatch();
  const definition = useDefinition("ScheduleForEdit");
  const texts = {
    title: "Schedules",
    subTitle: "Add a new Schedule",
  };

  function handleCreate() {
    dispatch(retrieveAll());
  }

  return <PageNew definition={definition} baseApiUrl="Schedules" texts={texts} onCreate={handleCreate} />;
}
