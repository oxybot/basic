import { useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";
import { retrieveAll } from "./slice";

export function EventCategoryEdit({ full = false }) {
  const dispatch = useDispatch();
  const { categoryId } = useParams();
  const definition = useDefinition("EventCategoryForEdit");
  const [, entity] = useApiFetch(["EventCategories", categoryId], { method: "GET" }, {});
  const texts = {
    title: entity.displayName,
    subTitle: "Edit a Category",
  };

  function handleUpdate() {
    dispatch(retrieveAll());
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
