import { useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";
import { refresh } from "./slice";

export function GlobalDayOffEdit({ full = false }) {
  const dispatch = useDispatch();
  const { dayOffId } = useParams();
  const definition = useDefinition("GlobalDayOffForEdit");
  const [, entity] = useApiFetch(["GlobalDaysOff", dayOffId], { method: "GET" }, {});
  const texts = {
    title: entity.displayName,
    subTitle: "Edit a Day-Off",
  };

  function handleUpdate() {
    dispatch(refresh());
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
