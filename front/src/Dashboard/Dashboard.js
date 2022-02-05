import clsx from "clsx";
import pluralize from "pluralize";
import { useApiFetch, useDefinition } from "../api";
import EntityList from "../Generic/EntityList";
import MobilePageTitle from "../Generic/MobilePageTitle";

function ProgressBar({ color, value, max, title }) {
  const text = `${title}: ${pluralize("hour", value, true)}`;
  return (
    <div
      className={clsx("progress-bar", color)}
      style={{ width: `${(value / max) * 100}%` }}
      role="progressbar"
      title={text}
    >
      <span className="visually-hidden">{text}</span>
    </div>
  );
}

function CardForConsumption() {
  const [loading, consumptions] = useApiFetch("Users/me/consumption", { method: "GET" });
  return (
    <div className="card">
      <div className="card-body">
        <h2 className="card-title">My Time-off Balances</h2>
        {!loading &&
          consumptions.map((consumption) => {
            const withBalance = consumption.allowed !== null;
            const balanceTotal = (consumption.allowed || 0) + (consumption.transfered || 0);
            const registeredTotal = consumption.taken + consumption.planned + consumption.requested;
            const max = Math.max(balanceTotal, registeredTotal);
            return (
              <div key={consumption.category.identifier} className="mb-2">
                <h4>{consumption.category.displayName}</h4>
                {withBalance && (
                  <div className="progress mb-1">
                    <ProgressBar color="bg-primary" value={consumption.allowed} max={max} title="Allowed" />
                    <ProgressBar color="bg-secondary" value={consumption.transfered} max={max} title="Transfered" />
                  </div>
                )}
                <div className="progress">
                  <ProgressBar color="bg-success" value={consumption.taken} max={max} title="Taken" />
                  <ProgressBar color="bg-orange" value={consumption.planned} max={max} title="Planned" />
                  <ProgressBar color="bg-red" value={consumption.requested} max={max} title="Requested" />
                </div>
                <table className="table">
                  <tbody>
                    {withBalance && (
                      <tr>
                        <td>Total Balance:</td>
                        <td className="text-end">{pluralize("hour", balanceTotal, true)}</td>
                      </tr>
                    )}
                    <tr>
                      <td>Total Registered:</td>
                      <td className="text-end">{pluralize("hour", registeredTotal, true)}</td>
                    </tr>
                    {withBalance && (
                      <tr>
                        <td>Remaining:</td>
                        <td className="text-end">{pluralize("hour", balanceTotal - registeredTotal, true)}</td>
                      </tr>
                    )}
                  </tbody>
                </table>
              </div>
            );
          })}
      </div>
    </div>
  );
}

function CardForEvents() {
  const [loading, events] = useApiFetch("Events/mine?limit=6", { method: "GET" }, []);
  let definition = useDefinition("EventForList");
  if (definition) {
    definition.fields = definition.fields.filter((f) => f.name !== "user");
  }

  return (
    <div className="card">
      <div className="card-body">
        <h2 className="card-title">My Latest Requests</h2>
        <EntityList loading={loading} definition={definition} entities={events} />
      </div>
    </div>
  );
}

export function Dashboard() {
  return (
    <>
      <MobilePageTitle>
        <div className="navbar-brand">Dashboard</div>
      </MobilePageTitle>
      <div className="container-xl">
        <div className="page-header d-print-none d-none d-lg-block">
          <div className="row align-items-center">
            <div className="col">
              <div className="page-pretitle">Overview</div>
              <h2 className="page-title">Dashboard</h2>
            </div>
          </div>
        </div>
        <div className="page-body">
          <div className="row row-cards">
            <div className="col-lg-6">
              <CardForConsumption />
            </div>
            <div className="col-lg-6">
              <CardForEvents />
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
