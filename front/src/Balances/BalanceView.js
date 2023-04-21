import { useLoaderData } from "react-router-dom";
import pluralize from "pluralize";
import { useDefinition } from "../api";
import EntityDetail from "../Generic/EntityDetail";
import PageView from "../Generic/PageView";
import Section from "../Generic/Section";
import Sections from "../Generic/Sections";

const transform = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "details");
  return d;
};

function BalanceViewDetail() {
  const definition = useDefinition("BalanceForView", transform);
  const entity = useLoaderData();
  const details = entity.details || [];

  return (
    <>
      <EntityDetail definition={definition} entity={entity} />
      <div className="card mb-3">
        <div className="card-header">
          <h3 className="card-title">Details</h3>
          <span className="badge ms-2 bg-green">{details.length || ""}</span>
          <span className="h2 mb-0 ms-auto">{pluralize("hour", entity.total, true)}</span>
        </div>
        <div className="card-body">
          {details.length === 0 && (
            <p>
              <em>No detail defined</em>
            </p>
          )}
          {details.map((item, index) => (
            <div key={index} className="row">
              <div className="col mb-3">
                <div className="lead">{item.description}</div>
              </div>
              <div className="col-auto">
                <span className="h4 border-secondary">{pluralize("hour", item.value, true)}</span>
              </div>
            </div>
          ))}
        </div>
      </div>
    </>
  );
}

export function BalanceView({ backTo = null, full = false }) {
  const entity = useLoaderData();

  return (
    <PageView backTo={backTo} full={full} entity={entity} title={entity.title} editRole="time">
      <Sections>
        <Section code="detail" element={<BalanceViewDetail />}>
          Detail
        </Section>
      </Sections>
    </PageView>
  );
}
