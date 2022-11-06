import { useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";
import { retrieveAll } from "./slice";

function transform(e) {
  let updated = { ...e, userIdentifier: e.user.identifier, categoryIdentifier: e.category.identifier };
  delete updated.user;
  delete updated.category;
  return updated;
}

export function BalanceEdit({ full = false }) {
  const dispatch = useDispatch();
  const { balanceId } = useParams();
  const definition = useDefinition("BalanceForEdit");
  const texts = {
    title: "Edit a Balance",
  };

  function handleUpdate() {
    dispatch(retrieveAll);
  }

  return (
    <PageEdit
      definition={definition}
      texts={texts}
      full={full}
      baseApiUrl="Balances"
      entityId={balanceId}
      onUpdate={handleUpdate}
      transform={transform}
    />
  );
}
