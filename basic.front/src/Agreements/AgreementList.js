import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import { agreementsState, disconnect, getAll } from "../Agreements/slice";
import PageList from "../Generic/PageList";

export function AgreementList() {
  const dispatch = useDispatch();
  const { agreementId } = useParams();
  const definition = useDefinition("AgreementForList");
  const texts = {
    title: "Agreements",
    add: "Add agreement",
  };

  const { loading, values: elements } = useSelector(agreementsState);

  useEffect(() => {
    dispatch(getAll());
    return () => dispatch(disconnect());
  }, [dispatch]);

  return (
    <PageList definition={definition} loading={loading} elements={elements} selectedId={agreementId} texts={texts} />
  );
}
