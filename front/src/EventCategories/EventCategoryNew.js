import { useRevalidator } from "react-router-dom";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";

export function EventCategoryNew() {
  const revalidator = useRevalidator();
  const definition = useDefinition("EventCategoryForEdit");
  const texts = {
    title: "Event Categories",
    subTitle: "Add a new Category",
  };

  function handleCreate() {
    revalidator.revalidate();
  }

  return <PageNew definition={definition} baseApiUrl="EventCategories" texts={texts} onCreate={handleCreate} />;
}
