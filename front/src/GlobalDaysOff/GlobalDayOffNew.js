import { useDispatch } from "react-redux";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import { retrieveAll } from "./slice";

export function GlobalDayOffNew() {
  const dispatch = useDispatch();
  const definition = useDefinition("GlobalDayOffForEdit");
  const texts = {
    title: "Global Days-Off",
    subTitle: "Add a new Day-Off",
  };

  function handleCreate() {
    dispatch(retrieveAll());
  }

  return <PageNew definition={definition} baseApiUrl="GlobalDaysOff" texts={texts} onCreate={handleCreate} />;
}
