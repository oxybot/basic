import { useDispatch } from "react-redux";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import { refresh } from "./slice";

export function EventNew() {
  const dispatch = useDispatch();
  const definition = useDefinition("EventForEdit");
  const texts = {
    title: "Events",
    subTitle: "Add a new Event",
  };

  function handleCreate() {
    dispatch(refresh());
  }

  return <PageNew definition={definition} baseApiUrl="Events/notify" texts={texts} onCreate={handleCreate} />;
}
