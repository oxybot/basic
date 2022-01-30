import { useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";
import { refresh } from "./slice";

function transform(e) {
  let updated = { ...e, userIdentifier: e.user.identifier, categoryIdentifier: e.category.identifier };
  delete updated.user;
  delete updated.category;
  return updated;
}

export function EventEdit({ full = false }) {
  const dispatch = useDispatch();
  const { eventId } = useParams();
  const definition = useDefinition("EventForEdit");
  const texts = {
    title: (e) => e.displayName,
    subTitle: "Edit an Event",
  };

  function handleUpdate() {
    dispatch(refresh());
  }

  return (
    <PageEdit
      definition={definition}
      texts={texts}
      full={full}
      baseApiUrl="Events"
      entityId={eventId}
      onUpdate={handleUpdate}
      transform={transform}
    />
  );
}
