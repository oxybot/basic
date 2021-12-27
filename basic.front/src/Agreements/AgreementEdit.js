import { useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";
import { refresh } from "./slice";

export function AgreementEdit({ full = false }) {
  const dispatch = useDispatch();
  const { agreementId } = useParams();
  const definition = useDefinition("AgreementForEdit");

  const transform = (e) => {
    const t = { ...e, clientIdentifier: e.client.identifier };
    delete t.client;
    return t;
  };

  const texts = {
    title: (e) => e.title,
    subTitle: "Edit an Agreement",
  };

  function handleUpdate() {
    dispatch(refresh());
  }

  return (
    <PageEdit
      definition={definition}
      baseApiUrl="Agreements"
      entityId={agreementId}
      full={full}
      texts={texts}
      transform={transform}
      onUpdate={handleUpdate}
    />
  );
}
