import { useParams, useRevalidator } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";

export function EventCategoryEdit({ full = false }) {
  const revalidator = useRevalidator();
  const { categoryId } = useParams();
  const definition = useDefinition("EventCategoryForEdit");
  const [, entity] = useApiFetch(["EventCategories", categoryId], { method: "GET" }, {});
  const texts = {
    title: entity.displayName,
    subTitle: "Edit a Category",
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
      baseApiUrl="EventCategories"
      entityId={categoryId}
      onUpdate={handleUpdate}
    />
  );
}
