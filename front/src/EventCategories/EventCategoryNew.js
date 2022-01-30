import { useDispatch } from "react-redux";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import { refresh } from "./slice";

export function EventCategoryNew() {
  const dispatch = useDispatch();
  const definition = useDefinition("EventCategoryForEdit");
  const texts = {
    title: "Event Categories",
    subTitle: "Add a new Category",
  };

  function handleCreate() {
    dispatch(refresh());
  }

  return <PageNew definition={definition} baseApiUrl="EventCategories" texts={texts} onCreate={handleCreate} />;
}
