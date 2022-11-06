import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";
import { agreementsState, disconnect, retrieveAll, setSorting } from "./slice";

export function AgreementList() {
  const dispatch = useDispatch();
  const { agreementId } = useParams();
  const definition = useDefinition("AgreementForList");
  const texts = {
    title: "Agreements",
    add: "Add agreement",
  };

  const { loading, elements, sorting } = useSelector(agreementsState);

  useEffect(() => {
    dispatch(retrieveAll());
    return () => disconnect();
  }, [dispatch, sorting]);

  return (
    <PageList
      definition={definition}
      loading={loading}
      elements={elements}
      selectedId={agreementId}
      texts={texts}
      newRole="client"
      sorting={sorting}
      setSorting={(s) => dispatch(setSorting(s))}
    />
  );
}
